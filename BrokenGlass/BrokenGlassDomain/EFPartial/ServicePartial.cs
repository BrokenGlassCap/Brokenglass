using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BrokenGlassDomain
{
    [MetadataType(typeof(ServicesMetadata))]
    public partial class Service
    {
        internal class ServicesMetadata
        {
            [DisplayName("ID")]
            public int Id { get; set; }
            [DisplayName("Код")]
            public string Code { get; set; }
            [DisplayName("Наименование")]
            public string Name { get; set; }
            [DisplayName("Порядок сортировки")]
            public Nullable<int> Order { get; set; }
            [DisplayName("Последние изменение")]
            public Nullable<System.DateTime> UpdateAt { get; set; }
            [DisplayName("Кто изменил")]
            public string UpdateBy { get; set; }

        }
    }

    
}
