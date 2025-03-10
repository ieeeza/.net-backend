using GameStore.Dtos;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSwaggerGen();

var app = builder.Build();

const string GetGameEndpointName = "GetGame";

List<GameDto> games = [
    new (
        1,
        "LOL",
        "MOBA",
        00.00M,
        new DateOnly(2009, 09, 02)
    ),
    new (
        2,
        "VALORANT",
        "FPS",
        10.99M,
        new DateOnly(2020, 02, 24)
    ),
    ];

app.MapGet("games", () => games);

app.MapGet("games/{id}", (int id) =>
{
    GameDto? game = games.Find(x => x.Id == id);

    return game is null ? Results.NoContent() : Results.Ok(game);
}).WithName(GetGameEndpointName);

app.MapPost("games", (CreateGameDto newGame) =>
{
    GameDto game = new(
        games.Count + 1,
        newGame.Name,
        newGame.Genre,
        newGame.Price,
        newGame.ReleaseDate
    );

    games.Add(game);

    return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
});

app.MapPut("games/{id}", (int id, UpdateGameDto updateGame) =>
{
    var index = games.FindIndex(x => x.Id == id);

    if (index == -1)
    {
        return Results.NoContent();
    }

    games[index] = new GameDto(
        id,
        updateGame.Name,
        updateGame.Genre,
        updateGame.Price,
        updateGame.ReleaseDate
    );

    return Results.NoContent();
});

app.MapDelete("games/{id}", (int id) =>
{
    games.RemoveAll(x => x.Id == id);

    return Results.NoContent();
});

app.MapGet("/", () => "Hello World!");

app.UseSwagger();
app.UseSwaggerUI();

await app.RunAsync();
