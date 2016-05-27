using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace BrokenGlassDomain
{
    [MetadataType(typeof(ServicesMetadata))]
    [DataContract]
    public partial class Service
    {
        internal class ServicesMetadata
        {
            [DataMember]
            [DisplayName("ID")]
            public int Id { get; set; }
            [DataMember]
            [DisplayName("Код")]
            public string Code { get; set; }
            [DisplayName("Наименование")]
            [DataMember]
            public string Name { get; set; }
            [DisplayName("Порядок сортировки")]
            [DataMember]
            public Nullable<int> Order { get; set; }
            [DataMember]
            [DisplayName("Последние изменение")]
            public Nullable<System.DateTime> UpdateAt { get; set; }
            [DataMember]
            [DisplayName("Кто изменил")]
            public string UpdateBy { get; set; }
            public virtual ICollection<Claim> Claim { get; set; }

        }
    }

    [MetadataType(typeof(ClaimStateMetadata))]
    [DataContract]
    public partial class ClaimState
    {
        internal class ClaimStateMetadata
        {
            [DataMember]
            public int Id { get; set; }
            [DataMember]
            public string Code { get; set; }
            [DataMember]
            public string Name { get; set; }
            [DataMember]
            public Nullable<System.DateTime> UpdateAt { get; set; }
            [DataMember]
            public string UpdateBy { get; set; }
            public virtual ICollection<Claim> Claim { get; set; }

        }
    }

    [MetadataType(typeof(AdressMetadata))]
    [DataContract]
    public partial class Adress
    {
        internal class AdressMetadata
        {
            [DataMember]
            public int Id { get; set; }
            [DataMember]
            public int OsbCode { get; set; }
            [DataMember]
            public string OsbName { get; set; }
            [DataMember]
            public string City { get; set; }
            [DataMember]
            public string AdressName { get; set; }
            [DataMember]
            public string Location { get; set; }
            [DataMember]
            public decimal Latitude { get; set; }
            [DataMember]
            public decimal Longitude { get; set; }
            [DataMember]
            public Nullable<System.DateTime> UpdateAt { get; set; }
            [DataMember]
            public string UpdateBy { get; set; }
            public virtual ICollection<Claim> Claim { get; set; }

        }
    }

    [MetadataType(typeof(ClaimMetadata))]
    [DataContract]
    public partial class Claim
    {
        public bool HasPhotos { get; set; }
        internal class ClaimMetadata
        {
            [DataMember]
            public int Id { get; set; }
            [DataMember]
            public int ServiceId { get; set; }
            [DataMember]
            public int AdressId { get; set; }
            [DataMember]
            public int UserId { get; set; }
            [DataMember]
            public int ClaimStateId { get; set; }
            [DataMember]
            public string Description { get; set; }
            [DataMember]
            public string Solution { get; set; }
            [DataMember]
            public System.DateTime CreateAt { get; set; }
            [DataMember]
            public Nullable<System.DateTime> UpdateAt { get; set; }
            [DataMember]
            public string UpdateBy { get; set; }
            public virtual Adress Adress { get; set; }
            public virtual ClaimState ClaimState { get; set; }
            public virtual Service Service { get; set; }
            public virtual User User { get; set; }
            [DataMember]
            public virtual ICollection<Photo> Photo { get; set; }
            [DataMember]
            public bool HasPhotos { get; set; }
        }
    }


}
