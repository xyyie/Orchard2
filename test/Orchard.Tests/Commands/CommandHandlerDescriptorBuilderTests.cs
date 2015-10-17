﻿using Orchard.Environment.Commands;
using System.Linq;
using Xunit;
using System.Reflection;

namespace Orchard.Tests.Commands {
    public class CommandHandlerDescriptorBuilderTests {
        [Fact]
        public void BuilderShouldCreateDescriptor() {
            var builder = new CommandHandlerDescriptorBuilder();
            var descriptor = builder.Build(typeof(MyCommand));
            Assert.NotNull(descriptor);
            Assert.Equal(4, descriptor.Commands.Count());
            Assert.NotNull(descriptor.Commands.SingleOrDefault(d => d.Name == "FooBar"));
            Assert.Equal(typeof(MyCommand).GetMethod("FooBar"), descriptor.Commands.Single(d => d.Name == "FooBar").MethodInfo);
            Assert.NotNull(descriptor.Commands.SingleOrDefault(d => d.Name == "MyCommand"));
            Assert.Equal(typeof(MyCommand).GetMethod("FooBar2"), descriptor.Commands.Single(d => d.Name == "MyCommand").MethodInfo);
            Assert.NotNull(descriptor.Commands.SingleOrDefault(d => d.Name == "Foo Bar"));
            Assert.Equal(typeof(MyCommand).GetMethod("Foo_Bar"), descriptor.Commands.Single(d => d.Name == "Foo Bar").MethodInfo);
            Assert.NotNull(descriptor.Commands.SingleOrDefault(d => d.Name == "Foo_Bar"));
            Assert.Equal(typeof(MyCommand).GetMethod("Foo_Bar3"), descriptor.Commands.Single(d => d.Name == "Foo_Bar").MethodInfo);
        }

        public class MyCommand : DefaultOrchardCommandHandler {
            public void FooBar() {
            }

            [CommandName("MyCommand")]
            public void FooBar2() {
            }

            public void Foo_Bar() {
            }

            [CommandName("Foo_Bar")]
            public void Foo_Bar3() {
            }
        }

        [Fact]
        public void BuilderShouldReturnPublicMethodsOnly() {
            var builder = new CommandHandlerDescriptorBuilder();
            var descriptor = builder.Build(typeof(PublicMethodsOnly));
            Assert.NotNull(descriptor);
            Assert.Equal(1, descriptor.Commands.Count());
            Assert.NotNull(descriptor.Commands.SingleOrDefault(d => d.Name == "Method"));
        }

#pragma warning disable 660,661
        public class PublicMethodsOnly {
#pragma warning restore 660,661
            public bool Bar { get; set; }   // no accessors
            public bool Field = true;       // no field

            // no private method
            private void Blah() {
            }

            // no private method
            public static void Foo() {
            }

            // no operator
            public static bool operator ==(PublicMethodsOnly a, PublicMethodsOnly b) {
                return false;
            }

            public static bool operator !=(PublicMethodsOnly a, PublicMethodsOnly b) {
                return false;
            }

            public void Method() {
            }
        }

    }
}