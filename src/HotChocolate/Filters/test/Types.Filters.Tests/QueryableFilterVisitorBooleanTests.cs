using System;
using System.Threading.Tasks;
using HotChocolate.Language;
using HotChocolate.Utilities;
using Xunit;
using static HotChocolate.Tests.TestHelper;


namespace HotChocolate.Types.Filters
{
    public class QueryableFilterVisitorBooleanTests
    {
        [Fact]
        public async Task Create_BooleanEqual_Expression()
        {
            // arrange
            var value = new ObjectValueNode(
                new ObjectFieldNode("bar",
                    new BooleanValueNode(true)));

            var fooType = await CreateTypeAsync(new FooFilterType());

            // act
            var filter = new QueryableFilterVisitor(
                fooType, typeof(Foo), TypeConversion.Default);
            value.Accept(filter);
            Func<Foo, bool> func = filter.CreateFilter<Foo>().Compile();

            // assert
            var a = new Foo { Bar = true };
            Assert.True(func(a));

            var b = new Foo { Bar = false };
            Assert.False(func(b));
        }

        [Fact]
        public async Task Create_BooleanNotEqual_Expression()
        {
            // arrange
            var value = new ObjectValueNode(
                new ObjectFieldNode("bar",
                    new BooleanValueNode(false)));

            var fooType = await CreateTypeAsync(new FooFilterType());

            // act
            var filter = new QueryableFilterVisitor(
                fooType, typeof(Foo), TypeConversion.Default);
            value.Accept(filter);
            Func<Foo, bool> func = filter.CreateFilter<Foo>().Compile();

            // assert
            var a = new Foo { Bar = false };
            Assert.True(func(a));

            var b = new Foo { Bar = true };
            Assert.False(func(b));
        }

        [Fact]
        public async Task Create_NullableBooleanEqual_Expression()
        {
            // arrange
            var value = new ObjectValueNode(
                new ObjectFieldNode("bar",
                    new BooleanValueNode(true)));

            var fooNullableType = await CreateTypeAsync(new FooNullableFilterType());

            // act
            var filter = new QueryableFilterVisitor(
                fooNullableType, typeof(FooNullable), TypeConversion.Default);
            value.Accept(filter);
            Func<FooNullable, bool> func = filter.CreateFilter<FooNullable>().Compile();

            // assert
            var a = new FooNullable { Bar = true };
            Assert.True(func(a));

            var b = new FooNullable { Bar = false };
            Assert.False(func(b));

            var c = new FooNullable { Bar = null };
            Assert.False(func(c));
        }

        [Fact]
        public async Task Create_NullableBooleanNotEqual_Expression()
        {
            // arrange
            var value = new ObjectValueNode(
                new ObjectFieldNode("bar",
                    new BooleanValueNode(false)));

            var fooNullableType = await CreateTypeAsync(new FooNullableFilterType());

            // act
            var filter = new QueryableFilterVisitor(
                fooNullableType, typeof(FooNullable), TypeConversion.Default);
            value.Accept(filter);
            Func<FooNullable, bool> func = filter.CreateFilter<FooNullable>().Compile();

            // assert
            var a = new FooNullable { Bar = false };
            Assert.True(func(a));

            var b = new FooNullable { Bar = true };
            Assert.False(func(b));

            var c = new FooNullable { Bar = null };
            Assert.False(func(c));
        }

        public class Foo
        {
            public bool Bar { get; set; }
        }

        public class FooNullable
        {
            public bool? Bar { get; set; }
        }

        public class FooFilterType
            : FilterInputType<Foo>
        {
            protected override void Configure(
                IFilterInputTypeDescriptor<Foo> descriptor)
            {
                descriptor.Filter(t => t.Bar)
                    .AllowEquals().And().AllowNotEquals();
            }
        }

        public class FooNullableFilterType
            : FilterInputType<FooNullable>
        {
            protected override void Configure(
                IFilterInputTypeDescriptor<FooNullable> descriptor)
            {
                descriptor.Filter(t => t.Bar)
                    .AllowEquals().And().AllowNotEquals();
            }
        }
    }
}
