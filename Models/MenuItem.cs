using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HierarchicalCatalog.Models
{
    public class MenuItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }// отлючена автоматическая генерация id

        public string Header { get; set; }  // заголовок меню
        public int? Order { get; set; }  // порядок следования пункта в подменю
        public int? ParentId { get; set; }  // ссылка на id родительского меню

        public MenuItem()
        {
        }
    }
}