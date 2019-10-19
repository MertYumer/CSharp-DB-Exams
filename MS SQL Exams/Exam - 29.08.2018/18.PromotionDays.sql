 CREATE FUNCTION udf_GetPromotedProducts(@CurrentDate DATETIME2, @StartDate DATETIME2, @EndDate DATETIME2, 
        @Discount INT, @FirstItemId INT, @SecondItemId INT, @ThirdItemId INT)
RETURNS NVARCHAR(200)
  BEGIN
        DECLARE @FirstItemPrice DECIMAL(15, 2) = (SELECT i.Price
													FROM Items i
												   WHERE i.Id = @FirstItemId)

        DECLARE @SecondItemPrice DECIMAL(15, 2) = (SELECT i.Price
													FROM Items i
												   WHERE i.Id = @SecondItemId)

        DECLARE @ThirdItemPrice DECIMAL(15, 2) = (SELECT i.Price
													FROM Items i
												   WHERE i.Id = @ThirdItemId)

		IF(@FirstItemPrice IS NULL OR @SecondItemPrice IS NULL OR @ThirdItemPrice IS NULL)
		BEGIN
		      RETURN 'One of the items does not exists!'
		END

		IF(@CurrentDate NOT BETWEEN @StartDate AND @EndDate)
		BEGIN
		      RETURN 'The current date is not within the promotion dates!'
		END

		DECLARE @FirstItemNewPrice DECIMAL(15, 2) = @FirstItemPrice - (@FirstItemPrice * @Discount / 100)
		DECLARE @SecondItemNewPrice DECIMAL(15, 2) = @SecondItemPrice - (@SecondItemPrice * @Discount / 100)
		DECLARE @ThirdItemNewPrice DECIMAL(15, 2) = @ThirdItemPrice - (@ThirdItemPrice * @Discount / 100)

		DECLARE @FirstItemName NVARCHAR(30) = (SELECT i.Name
												 FROM Items i
												WHERE i.Id = @FirstItemId)

        DECLARE @SecondItemName NVARCHAR(30) = (SELECT i.Name
												  FROM Items i
												 WHERE i.Id = @SecondItemId)

        DECLARE @ThirdItemName NVARCHAR(30) = (SELECT i.Name
												 FROM Items i
												WHERE i.Id = @ThirdItemId)

        DECLARE @Result NVARCHAR(200) = @FirstItemName + ' price: ' + CAST(ROUND(@FirstItemNewPrice, 2) AS NVARCHAR) + ' <-> ' +
		                                @SecondItemName + ' price: ' + CAST(ROUND(@SecondItemNewPrice, 2) AS NVARCHAR) + ' <-> ' +
										@ThirdItemName + ' price: ' + CAST(ROUND(@ThirdItemNewPrice, 2) AS NVARCHAR)
		RETURN @Result
    END