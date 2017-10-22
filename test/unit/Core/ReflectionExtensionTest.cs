using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using RapidCore.Reflection;
using Skarp.HubSpotClient.Company;
using Skarp.HubSpotClient.Contact.Dto;
using Skarp.HubSpotClient.Core;
using Xunit;

namespace Skarp.HubSpotClient.UnitTest.Core
{
    public class ReflectionExtensionTest
    {


        [Fact]
        public void ReflectionExtension_resolves_prop_names_correctly()
        {
            var dto = new ClassWithDataMembers();
            var dtoProps = dto.GetType().GetProperties();

            var propNoCustomName = dtoProps.Single(p => p.Name == "MemberNoCustomName");
            Assert.Equal("MemberNoCustomName", propNoCustomName.GetPropSerializedName());
            var propWithCustomName = dtoProps.Single(p => p.Name == "MemberWithCustomName");
            Assert.Equal("CallMeThis", propWithCustomName.GetPropSerializedName());
        }

        [Fact]
        public void ReflectionExtension_resolves_prop_names_correctly_handles_null_prop()
        {
            PropertyInfo prop = null;
            Assert.Null(prop.GetPropSerializedName());
        }

        [Fact]
        public void ReflectionExtension_can_find_methods_from_interfaces()
        {
            var dto = new ContactListHubSpotEntity<ContactHubSpotEntity>();

            // find the Add(T) method on the List entity!
            var listProp = dto.GetType().GetProperties().Single(p => p.Name == "Contacts");
            var method = listProp.PropertyType.FindMethodRecursively("Add", new[] { typeof(ContactHubSpotEntity) });

            Assert.NotNull(method);
        }

        [Fact]
        public void ReflectionExtension_can_find_methods_from_base_classes()
        {
            // all classes inherits from Object and Object has ToString() method!
            var instance = new Inheritor();
            var method = instance.GetType().FindMethodRecursively("ToString");

            Assert.NotNull(method);
        }

        [Fact]
        public void ReflectionExtension_throws_when_unable_to_find_method()
        {
            // all classes inherits from Object and Object has ToString() method!
            var instance = new Inheritor();
            var exception = Record.Exception(() => instance.GetType().FindMethodRecursively("IfThisIsHereStuffIsWhack"));

            Assert.NotNull(exception);
            var argumentException = Assert.IsType<ArgumentException>(exception);

            Assert.Equal("name", argumentException.ParamName);
        }


        [Fact]
        public void ReflectionExtension_can_find_methods_from_base_types_and_interfaces_handles_null()
        {
            Type type = null;
            Assert.Null(type.FindMethodRecursively("Add"));
        }

        [Fact]
        public void ReflectionExtension_can_determine_if_prop_has_ignore_data_member()
        {
            var dto = new ClassWithDataMembers();
            var propWithIgnore = dto.GetType().GetProperties().Single(p => p.Name == "IgnoreMePlease");
            var hasAttr = propWithIgnore.HasIgnoreDataMemberAttribute();
            Assert.True(hasAttr);

            var propNoIgnore = dto.GetType().GetProperties().Single(p => p.Name == "MemberWithCustomName");
            Assert.False(propNoIgnore.HasIgnoreDataMemberAttribute());
        }

        [Fact]
        public void ReflectionExtension_can_determine_if_prop_has_ignore_data_member_handles_null()
        {
            PropertyInfo prop = null;
            Assert.False(prop.HasIgnoreDataMemberAttribute());
        }

		[Fact]
        public void ReflectionExtension_can_determine_if_stuff_is_complex()
        {
            string nuller = null;
            var str = " Hey I am a string!";
            var theAnswer = 42;

            var complexBeast = new CompanySearchByDomain();

            Assert.False(str.IsComplexType(), "str.IsComplexType()");
            Assert.False(theAnswer.IsComplexType(), "theAnswer.IsComplexType()");
            Assert.True(complexBeast.IsComplexType(), "complexBeast.IsComplexType()");
            Assert.False(nuller.IsComplexType());
        }

        [DataContract]
        private class ClassWithDataMembers
        {

            [DataMember()]
            public string MemberNoCustomName { get; set; }

            [DataMember(Name = "CallMeThis")]
            public string MemberWithCustomName { get; set; }

            [IgnoreDataMember]
            public string IgnoreMePlease { get; set; }
        }


        private class Inheritor
        {

        }
    }
}
