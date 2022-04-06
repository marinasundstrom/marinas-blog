Welcome to my blog!

## Code sample

```c#
public class MovieRepository
{
    public IReadOnlyList<Movie> Find(
        DateTime? maxReleaseDate = null, 
        double minRating = 0, 
        string genre = null)
    {
        /* ... */
    }
}
```