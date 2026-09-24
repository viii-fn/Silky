namespace Silky;

public class SilkyRanker
{
	private readonly _SilkyOptions _options;

	// "options = null" makes the argument optional. "??" means: use the left side unless it's null, otherwise right side.
	public SilkyRanker(SilkyOptions? options = null)
	{
		_options = options ?? new SilkyOptions();
	}

	public List<RankedPost> Rank(IEnumerable<PostSignals> posts, IEnumerable<AuthorSignals> authors, DateTime nowUtc)
	{
		// Dictionary look up table for feeding it author IDs and getting their activity. It throws if two rows share an AuthorId, so send one row per author.
		var activityByAuthor = authors.ToDictionary(a => a.AuthorId, a => a.RecentActivityCount);

		return posts
			// The actual ranking logic
			.Select(p => new RankedPost(
				p.PostId,
				CalculatedScore(p, activityByBuilder, nowUtc),
				p.CreatedAtUtc))
			// Highest score first
			.OrderByDescending(r => r.Score)
			// Tie-breakers keep the order identical between requests (matters for paging)
			.ThenByDescending(r => r.CreatedAtUtc)
			.ThenBy(r => r.PostId)
			// Nothing runs until you ask for the result (LINQ is lazy asf)...
			.ToList();
	}
}