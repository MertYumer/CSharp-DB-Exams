namespace VaporStore.DataProcessor
{
	using System;
	using System.Linq;

	using Data;
	using ExportDtos;
    using Newtonsoft.Json;
    using Data.Models.Enums;
    using System.Globalization;
    using System.Xml.Serialization;
    using System.Text;
    using System.Xml;
    using System.IO;

    public static class Serializer
	{
		public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
		{
            var genres = context
                .Genres
                .Where(g => genreNames.Any(gn => gn == g.Name))
                .OrderByDescending(g => g.Games.Sum(ga => ga.Purchases.Count))
                .ThenBy(g => g.Id)
                .ToArray();

            var genreDtos = genres
                .Select(g => new ExportGenreDto
                {
                    Id = g.Id,
                    Genre = g.Name,
                    Games = g.Games
                        .Where(ga => ga.Purchases.Any())
                        .OrderByDescending(ga => ga.Purchases.Count)
                        .ThenBy(ga => ga.Id)
                        .Select(ga => new ExportGameDto
                        { 
                            Id = ga.Id,
                            Title = ga.Name,
                            Developer = ga.Developer.Name,
                            Tags = ga
                                .GameTags
                                .Select(gt => gt.Tag.Name)
                                .Aggregate((i, j) => i + ", " + j),
                            Players = ga.Purchases.Count
                        })
                        .ToArray(),

                    TotalPlayers = g.Games.Sum(ga => ga.Purchases.Count)
                });

            var jsonResult = JsonConvert.SerializeObject(genreDtos, Newtonsoft.Json.Formatting.Indented);

            return jsonResult;
        }

		public static string ExportUserPurchasesByType(VaporStoreDbContext context, string storeType)
		{
            var userDtos = context
                .Users
                .Select(u => new ExportUserDto
                { 
                    Username = u.Username,

                    Purchases = u.Cards
                    .SelectMany(c => c.Purchases)
                    .Where(p => p.Type.ToString() == storeType)
                    .Select(p => new ExportPurchaseDto
                    { 
                        Card = p.Card.Number,
                        Cvc = p.Card.Cvc,
                        Date = p.Date.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                        Game = new ExportGameXmlDto
                        { 
                            Title = p.Game.Name,
                            Genre = p.Game.Genre.Name,
                            Price = p.Game.Price
                        }
                    })
                    .OrderBy(pd => pd.Date)
                    .ToArray(),

                    TotalSpent = u.Cards
                    .SelectMany(c => c.Purchases)
                        .Where(p => p.Type.ToString() == storeType)
                        .Sum(p => p.Game.Price)
                })
                .Where(ud => ud.Purchases.Any())
                .OrderByDescending(ud => ud.TotalSpent)
                .ThenBy(ud => ud.Username)
                .ToArray();

            var xmlSerializer = new XmlSerializer(typeof(ExportUserDto[]),
                            new XmlRootAttribute("Users"));

            var stringBuilder = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            xmlSerializer.Serialize(new StringWriter(stringBuilder), userDtos, namespaces);

            return stringBuilder.ToString().TrimEnd();
        }
	}
}