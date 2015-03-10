
﻿using Fixie;
using System;
﻿using System.Collections.Generic;
﻿using System.Linq;
using System.Reflection;
﻿using NUnit.Framework;

namespace Ceilingfish.Picture.Core.Tests
{
    public class LowCeremonyConvention : Convention
    {
        static readonly string[] LifecycleMethods = { "FixtureSetUp", "FixtureTearDown", "SetUp", "TearDown" };

        public LowCeremonyConvention()
        {
            Classes
                .NameEndsWith("Tests");

            Methods
                .Where(method => method.IsVoid())
                .Where(method => LifecycleMethods.All(x => x != method.Name));

            ClassExecution
                .CreateInstancePerClass()
                .SortCases((caseA, caseB) => String.Compare(caseA.Name, caseB.Name, StringComparison.Ordinal));

            FixtureExecution
                .Wrap<CallFixtureSetUpTearDownMethodsByName>();

            CaseExecution
                .Wrap<CallSetUpTearDownMethodsByName>();
        }

        class CallSetUpTearDownMethodsByName : CaseBehavior
        {
            public void Execute(Case @case, Action next)
            {
                @case.Class.TryInvoke("SetUp", @case.Fixture.Instance);
                next();
                @case.Class.TryInvoke("TearDown", @case.Fixture.Instance);
            }
        }

        class CallFixtureSetUpTearDownMethodsByName : FixtureBehavior
        {
            public void Execute(Fixture fixture, Action next)
            {
                fixture.Class.Type.TryInvoke("FixtureSetUp", fixture.Instance);
                next();
                fixture.Class.Type.TryInvoke("FixtureTearDown", fixture.Instance);
            }
        }
    }

    public static class BehaviorBuilderExtensions
    {
        public static void InvokeAll<TAttribute>(this Type type, object instance)
            where TAttribute : Attribute
        {
            foreach (var method in Has<TAttribute>(type))
            {
                try
                {
                    method.Invoke(instance, null);
                }
                catch (TargetInvocationException exception)
                {
                    throw new PreservedException(exception.InnerException);
                }
            }
        }

        static IEnumerable<MethodInfo> Has<TAttribute>(Type type)
            where TAttribute : Attribute
        {
            return type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.HasOrInherits<TAttribute>());
        }

        public static void TryInvoke(this Type type, string method, object instance)
        {
            var lifecycleMethods =
                type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .Where(x => x.HasSignature(typeof(void), method))
                    .OrderBy(m =>
                    {
                        var currentType = m.DeclaringType;
                        int count = 0;
                        while (typeof(object) != currentType)
                        {
                            currentType = currentType.BaseType;
                            count++;
                        }

                        return count;
                    });

            var problems = new List<Exception>();
            foreach (var lifeCycleMethod in lifecycleMethods)
            {
                try
                {
                    lifeCycleMethod.Invoke(instance, null);
                }
                catch (TargetInvocationException exception)
                {
                    problems.Add(exception);
                }
            }

            if (problems.Any())
                throw new AggregateException(string.Format("Problem setting up {0} for type {1}", method, type.FullName), problems);

        }
    }
}