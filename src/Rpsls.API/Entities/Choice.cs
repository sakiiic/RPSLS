using System.ComponentModel.DataAnnotations.Schema;

namespace Rpsls.API.Entities
{
    public class Choice
    {       
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<GameRule> WinningOutcomes { get; set; }
        public virtual ICollection<GameRule> LosingOutcomes { get; set; }

        public bool ChoiceExists(int userChoice) {

            return userChoice == Id;
        }
    }
}
