namespace VaporStore.DataProcessor
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.Globalization;
	using System.Linq;
	using System.Text;

	using Data;
	using Data.Models;
	using Newtonsoft.Json;
	using Dto;
    using VaporStore.Data.Models.Enums;
    using System.Xml.Serialization;
    using System.IO;

    public static class Deserializer
	{
        private const string ErrorMessage = "Invalid Data";

        private const string SuccessfullyImportedGame
            = "Added {0} ({1}) with {2} tags";

        private const string SuccessfullyImportedUser
            = "Imported {0} with {1} cards";

        private const string SuccessfullyImportedPurchase
            = "Imported {0} for {1}";

        public static string ImportGames(VaporStoreDbContext context, string jsonString)
		{
            var games = new HashSet<Game>();
            var developers = new HashSet<Developer>();
            var genres = new HashSet<Genre>();
            var tags = new HashSet<Tag>();

            var gameDtos = JsonConvert.DeserializeObject<ImportGameDto[]>(jsonString);
            var sb = new StringBuilder();

            foreach (var gameDto in gameDtos)
            {
                var isValidGame = IsValid(gameDto);
                var areValidTags = gameDto.Tags.Any();

                if (!isValidGame || !areValidTags)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var developer = developers.Any(d => d.Name == gameDto.Developer)
                        ? developers.FirstOrDefault(d => d.Name == gameDto.Developer)
                        : new Developer { Name = gameDto.Developer, };

                var genre = genres.Any(g => g.Name == gameDto.Genre)
                        ? genres.FirstOrDefault(g => g.Name == gameDto.Genre)
                        : new Genre { Name = gameDto.Genre, };

                developers.Add(developer);
                genres.Add(genre);

                var currentTags = new HashSet<Tag>();

                foreach (var tagName in gameDto.Tags)
                {
                    var tag = tags.Any(t => t.Name == tagName)
                        ? tags.FirstOrDefault(t => t.Name == tagName)
                        : new Tag { Name = tagName };

                    currentTags.Add(tag);
                    tags.Add(tag);
                }

                var game = new Game
                {
                    Name = gameDto.Name,
                    Price = gameDto.Price,
                    ReleaseDate = DateTime.ParseExact(gameDto.ReleaseDate, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    Developer = developer,
                    Genre = genre,
                    GameTags = currentTags
                    .Select(t => new GameTag { Tag = t })
                    .ToList()
                };

                games.Add(game);
                sb.AppendLine(string.Format(SuccessfullyImportedGame, game.Name, game.Genre.Name, game.GameTags.Count));
            }

            context.Games.AddRange(games);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

		public static string ImportUsers(VaporStoreDbContext context, string jsonString)
		{
            var users = new HashSet<User>();
            var userDtos = JsonConvert.DeserializeObject<ImportUserDto[]>(jsonString);

            var sb = new StringBuilder();

            foreach (var userDto in userDtos)
            {
                var isValidUser = IsValid(userDto);
                var areValidCards = userDto.Cards.All(c => IsValid(c) && Enum.IsDefined(typeof(CardType), c.Type));

                if (!isValidUser || !areValidCards)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var cards = userDto
                    .Cards
                    .Select(c => new Card
                    {
                        Number = c.Number,
                        Cvc = c.Cvc,
                        Type = (CardType)Enum.Parse(typeof(CardType), c.Type)
                    })
                    .ToArray();

                var user = new User
                { 
                    FullName = userDto.FullName,
                    Username = userDto.Username,
                    Email = userDto.Email,
                    Age = userDto.Age,
                    Cards = cards
                };

                users.Add(user);
                sb.AppendLine(string.Format(SuccessfullyImportedUser, user.Username, user.Cards.Count));
            }

            context.Users.AddRange(users);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

		public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
		{
            var xmlSerializer = new XmlSerializer(typeof(ImportPurchaseDto[]),
                    new XmlRootAttribute("Purchases"));

            var purchaseDtos = (ImportPurchaseDto[])(xmlSerializer.Deserialize(new StringReader(xmlString)));
            var purchases = new HashSet<Purchase>();

            StringBuilder sb = new StringBuilder();

            foreach (var purchaseDto in purchaseDtos)
            {
                var isValidGame = context.Games.Any(g => g.Name == purchaseDto.Title);
                var isValidType = Enum.IsDefined(typeof(PurchaseType), purchaseDto.Type);
                var isValidCard = context.Cards.Any(c => c.Number == purchaseDto.Card);


                if (!IsValid(purchaseDto) || !isValidGame || !isValidType || !isValidCard)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var purchase = new Purchase
                {
                    Game = context.Games.First(g => g.Name == purchaseDto.Title),
                    Type = (PurchaseType)Enum.Parse(typeof(PurchaseType), purchaseDto.Type),
                    ProductKey = purchaseDto.Key,
                    Card = context.Cards.First(c => c.Number == purchaseDto.Card),
                    Date = DateTime.ParseExact(purchaseDto.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)
                };

                purchases.Add(purchase);
                sb.AppendLine(string.Format(
                        SuccessfullyImportedPurchase,
                        purchase.Game.Name,
                        purchase.Card.User.Username));
            }

            context.Purchases.AddRange(purchases);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object entity)
        {
            var validationContext = new ValidationContext(entity);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(entity, validationContext, validationResult, true);

            return isValid;
        }
    }
}