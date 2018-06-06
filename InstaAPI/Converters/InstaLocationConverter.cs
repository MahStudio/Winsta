using System;
using InstaSharper.Classes.Models;
using InstaSharper.Classes.ResponseWrappers;

namespace InstaSharper.Converters
{
    internal class InstaLocationConverter : IObjectConverter<InstaLocation, InstaLocationResponse>
    {
        public InstaLocationResponse SourceObject { get; set; }
        public InstaLocation Convert()
        {
            if (SourceObject == null) throw new ArgumentNullException($"Source object");
            var location = new InstaLocation
            {
                Name = SourceObject.Name,
                Address = SourceObject.Address,
                City = SourceObject.City,
                ExternalSource = SourceObject.ExternalIdSource,
                ExternalId = SourceObject.ExternalId,
                Lat = SourceObject.Lat,
                Lng = SourceObject.Lng,
                Pk = SourceObject.Pk,
                ShortName = SourceObject.ShortName
            };
            return location;
        }
    }
    internal class InstaLocationStoryConverter : IObjectConverter<InstaLocationStory, InstaLocationStoryResponse>
    {
        public InstaLocationStoryResponse SourceObject { get; set; }
        
        public InstaLocationStory Convert()
        {
            if (SourceObject == null) throw new ArgumentNullException($"Source object");
            var location = new InstaLocationStory
            {
                Location = new InnerLocation()
                {
                    Name = SourceObject.Location.Name,
                    Address = SourceObject.Location.Address,
                    City = SourceObject.Location.City,
                    Pk = SourceObject.Location.Pk,
                    ShortName = SourceObject.Location.ShortName,
                    ExternalSource = SourceObject.Location.ExternalSource,
                    //ExternalId = SourceObject.Location.ExternalId,
                    Lat = SourceObject.Location.Lat,
                    Lng = SourceObject.Location.Lng,
                },
                Height = SourceObject.Height,
                IsHidden = SourceObject.IsHidden,
                IsPinned = SourceObject.IsPinned,
                Rotation = SourceObject.Rotation,
                Width = SourceObject.Width,
                X = SourceObject.X,
                Y = SourceObject.Y,
                Z = SourceObject.Z

            };
            return location;
        }
    }
}