namespace TankLibrary.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Tank")]
    public partial class Tank
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int Stage { get; set; }

        [Required]
        public string Type { get; set; }

        public string PlaceOfOrigin { get; set; }

        public string Manufacturer { get; set; }

        public int? ServiceStartYear { get; set; }

        public int? ServiceEndYear { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Weight { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Length { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Width { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Height { get; set; }

        public short? Crew { get; set; }

        public string Armor { get; set; }

        public string MainArmament { get; set; }

        public string SecondaryArmament { get; set; }

        public string Engine { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Power { get; set; }

        public string Transmission { get; set; }

        public string Suspension { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? GroundClearance { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? FuelCapacity { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Range { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Speed { get; set; }

        public string Description { get; set; }

        public string ImagePath { get; set; }

        public string ImageFile { get; set; }
    }
}
