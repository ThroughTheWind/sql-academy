USE LearningDb;
GO

CREATE OR ALTER PROCEDURE academy.GetTradesByUser
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        t.Id,
        t.UserId,
        t.InstrumentId,
        t.Quantity,
        t.Price,
        t.TradedUtc
    FROM academy.Trades AS t
    WHERE t.UserId = @UserId
    ORDER BY t.TradedUtc DESC, t.Id DESC;
END;
GO

EXEC academy.GetTradesByUser @UserId = 1;
GO