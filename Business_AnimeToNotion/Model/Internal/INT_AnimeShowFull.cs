namespace Business_AnimeToNotion.Model.Internal
{
    public class INT_AnimeShowFull : INT_AnimeShowBase
    {
        public List<INT_AnimeShowRelation> Relations { get; set; }

        public INT_AnimeShowEdit? Edit { get; set; }
    }
}
