using Domain.Interfaces;

namespace Domain
{
    public class ItemInput : IItemInput 
    {
        public string TextInput { get; set; }

        public ItemInput(string text) {
            this.TextInput = text;
        }

        
    }
}
