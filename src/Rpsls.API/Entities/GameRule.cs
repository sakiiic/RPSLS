namespace Rpsls.API.Entities
{
    public class GameRule
    {
        public int Id { get; set; }
        public int WinnerId { get; set; }
        public int LoserId { get; set; }
        public string Description { get; set; }

        public virtual Choice Winner { get; set; }
        public virtual Choice Loser { get; set; }

        public Boolean HasResult() {

            if (Id == null) {
                return true;
            }
            return false;
        }
    }
}
