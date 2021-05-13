using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Skarp.HubSpotClient.Company.Interfaces.Properties;
using Skarp.HubSpotClient.Core.Interfaces;

namespace Skarp.HubSpotClient.Company.Dto.Properties
{
    [DataContract]
    public class CompanyPropertyListHubSpotEntity<T> : IHubSpotEntity, ICollection<T>
        where T : ICompanyPropertyHubSpotEntity
    {
        private List<T> Properties { get; } = new();

        public bool IsNameValue => false;

        public IEnumerator<T> GetEnumerator()
            => Properties.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(T item)
            => Properties.Add(item);

        public void Clear()
            => Properties.Clear();

        public bool Contains(T item)
            => Properties.Contains(item);

        public void CopyTo(T[] array, int arrayIndex)
            => Properties.CopyTo(array, arrayIndex);

        public bool Remove(T item)
            => Properties.Remove(item);

        public int Count
            => Properties.Count;

        public bool IsReadOnly
            => false;

        public void ToHubSpotDataEntity(ref dynamic dataEntity)
        {

        }

        public void FromHubSpotDataEntity(dynamic hubspotData)
        {

        }
    }
}
