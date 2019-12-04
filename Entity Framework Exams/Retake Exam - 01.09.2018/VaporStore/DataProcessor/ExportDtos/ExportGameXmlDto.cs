﻿namespace VaporStore.DataProcessor.ExportDtos
{
    using System.Xml.Serialization;

    [XmlType("Game")]
    public class ExportGameXmlDto
    {
        [XmlAttribute("title")]
        public string Title { get; set; }

        [XmlElement]
        public string Genre { get; set; }

        [XmlElement]
        public decimal Price { get; set; }
    }
}
