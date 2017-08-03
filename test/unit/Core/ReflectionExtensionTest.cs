using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using Skarp.HubSpotClient.Dto;
using Skarp.HubSpotClient.Requests;
using Xunit;

namespace Skarp.HubSpotClient.UnitTest
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
        public void ReflectionExtension_can_find_methods_from_base_types_and_interfaces()
        {
            var dto = new ContactListHubSpotEntity<ContactHubSpotEntity>();

            // find the Add(T) method on the List entity!
            var listProp = dto.GetType().GetProperties().Single(p => p.Name == "Contacts");
            var method = listProp.PropertyType.FindMethodRecursively("Add", new [] {typeof(ContactHubSpotEntity) });

            Assert.NotNull(method);
        }

        [DataContract]
        private class ClassWithDataMembers
        {

            [DataMember()]
            public string MemberNoCustomName { get; set; }

            [DataMember(Name = "CallMeThis")]
            public string MemberWithCustomName { get; set; }
        }
    }
}
