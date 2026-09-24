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
	}
}