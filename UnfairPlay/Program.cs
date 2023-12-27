using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

string line;
var irrelevantTeams = new HashSet<int>();
while ((line = Console.ReadLine()) != "-1")
{
    irrelevantTeams.Clear();
    var tokens = line.Split(' ');
    var numberOfTeams = int.Parse(tokens[0]);
    var gamesLeft = int.Parse(tokens[1]);
    var scores = Console.ReadLine();
    var teamScores = scores.Split(' ').Select(int.Parse).ToArray();
    var gamesToPlay = new List<(int team1, int team2)>();
    var teams = new List<Team>();

    for (var i = 0; i < numberOfTeams; i++)
    {
        teams.Add(new Team { Number = i + 1, Score = teamScores[i] });
    }

    for (var i = 0; i < gamesLeft; i++)
    {
        var toPlay = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
        if (toPlay[0] == numberOfTeams || toPlay[1] == numberOfTeams)
        {
            teams.Last().Score += 2;
        }

        gamesToPlay.Add((toPlay[0], toPlay[1]));
    }

    Console.ReadLine();

    var myTeam = teams.Last();
    foreach (var team in teams)
    {
        var score = team.Score;
        var gamesLeftForTeam = gamesToPlay.Count(x => x.team1 == team.Number || x.team2 == team.Number 
        && !(x.team1 == numberOfTeams || x.team2 == numberOfTeams));
        score += gamesLeftForTeam * 2;
        team.GamesLeft = gamesLeftForTeam;

        if (score < myTeam.Score)
        {
            irrelevantTeams.Add(team.Number);
        }
    }

    var possibility = GetPossibility(teams, gamesToPlay, 0, "");
    if (possibility == null)
    {
        Console.WriteLine("NO");
    }
    else
    {
        Console.WriteLine(possibility[..^1]);
    }
}

string? GetPossibility(List<Team> teams, List<(int team1, int team2)> gamesToPlay, int pointer, string result)
{
    var myTeam = teams.Last();
    if (teams.Take(teams.Count - 1).Any(x => x.Score >= myTeam.Score))
    {
        return null;
    }

    if (pointer == gamesToPlay.Count)
    {
        return result;
    }

    var (team1, team2) = gamesToPlay[pointer];
    var team1Class = teams[team1 - 1];
    var team2Class = teams[team2 - 1];
    if (team1 == teams.Count || team2 == teams.Count)
    {
        return GetPossibility(teams, gamesToPlay, pointer + 1, result + (team1 == teams.Count ? "0 " : "2 "));
    }

    if (team1Class.Score + team1Class.GamesLeft * 2 < myTeam.Score)
    {
        team1Class.IsIrrevelant = true;
    }

    if (team2Class.Score + team2Class.GamesLeft * 2 < myTeam.Score)
    {
        team2Class.IsIrrevelant = true;
    }

    if (irrelevantTeams.Contains(team1) || team1Class.IsIrrevelant)
    {
        var copy = new List<Team>(teams);
        copy[team2 - 1] = team2Class.GiveLoss();
        return GetPossibility(copy, gamesToPlay, pointer + 1, result + "0 ");
    }

    if (irrelevantTeams.Contains(team2) || team2Class.IsIrrevelant)
    {
        var copy = new List<Team>(teams);
        copy[team1 - 1] = team1Class.GiveLoss();
        return GetPossibility(copy, gamesToPlay, pointer + 1, result + "2 ");
    }

    var firstCopy = new List<Team>(teams)
    {
        [team1 - 1] = team1Class.GiveWin(),
        [team2 - 1] = team2Class.GiveLoss()
    };

    var secondCopy = new List<Team>(teams);
    secondCopy[team1 - 1] = team1Class.GiveDraw();
    secondCopy[team2 - 1] = team2Class.GiveDraw();

    var thirdCopy = new List<Team>(teams);
    thirdCopy[team1 - 1] = team1Class.GiveLoss();
    thirdCopy[team2 - 1] = team2Class.GiveWin();

    return GetPossibility(firstCopy, gamesToPlay, pointer + 1, result + "0 ")
        ?? GetPossibility(secondCopy, gamesToPlay, pointer + 1, result + "1 ")
        ?? GetPossibility(thirdCopy, gamesToPlay, pointer + 1, result + "2 ");

    /*var guesses = new[]
    {
        (firstCopy, new[] { firstCopy[team1 - 1].Score }, result + "0 "),
        (secondCopy, new[] { secondCopy[team1 - 1].Score, secondCopy[team2 - 1].Score }, result + "1 "),
        (thirdCopy, new[] { firstCopy[team2 - 1].Score }, result + "2 ")
    };

    foreach (var guess in guesses.OrderBy(x => x.Item2.Max()))
    {
        var guessResult = GetPossibility(guess.Item1, gamesToPlay, pointer + 1, guess.Item3);
        if (guessResult != null)
        {
            return guessResult;
        }
    }

    return null;*/
}

class Team
{
    public int Number { get; set; }

    public int Score { get; set; }

    public int GamesLeft { get; set; }

    public bool IsIrrevelant { get; set; }

    public Team GiveWin() => new() { Number = Number, Score = Score + 2, GamesLeft = GamesLeft - 1, IsIrrevelant = IsIrrevelant };

    public Team GiveDraw() => new() { Number = Number, Score = Score + 1, GamesLeft = GamesLeft - 1, IsIrrevelant = IsIrrevelant };

    public Team GiveLoss() => new() { Number = Number, Score = Score, GamesLeft = GamesLeft - 1, IsIrrevelant = IsIrrevelant };
}