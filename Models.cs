namespace Silky;

// Everything Silky needs to rank your post. edit this in respect to your app :)
public record PostSignals(Guid PostId, Guid AuthorId, DateTime CreatedAtUtc, int LikeCount);

// This line shows how active a user or author has been recently.. send one row per author only
public record AuthorSignals(Guid AuthorId, int RecentActivityCount);

// The organized list of ranked posts or what comes back. It's score and creation time (for tie breaks lol)
public record RankedPost