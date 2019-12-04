namespace VaporStore.DataProcessor
{
	using System;
	using System.Linq;

	using Data;
	using ExportDtos;
    using Newtonsoft.Json;

    public static class Serializer
	{
		public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
		{
            var genres = context
                .Genres
                .Where(g => genreNames.Contains(g.Name))
                .OrderByDescending(g => g.Games.Sum(ga => ga.Purchases.Count))
                .ThenBy(g => g.Id)
                .ToArray();

            var genreDtos = genres
                .Select(g => new ExportGenreDto
                {
                    Id = g.Id,
                    Genre = g.Name,
                    Games = g.Games
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
			throw new NotImplementedException();
		}
	}
}