using ScriptModule.Models;

namespace UserModule.Models
{
    /// <summary>
    /// Defines the producer entity which is also a type of User
    /// </summary>
    public class Producer : User
    {
        //public override string Bio { get; set; }
        ///// <summary>
        ///// The list of script writing posts created by the producer.
        ///// </summary>
        //public List<ScriptWritingPostByProducer> ScriptWritingPosts { get; set; } = [];


        /// <summary>
        /// The list of scripts purchased by the producer.
        /// </summary>
        public List<Script> PurchasedScripts { get; set; } = [];
        /// <summary>
        /// The list of shared scripts between the producer and writers.
        /// </summary>
        public List<SharedScript> SharedScripts { get; set; } = [];
        /// <summary>
        /// The list of transactions made by the producer, such as payments for scripts or services.
        /// </summary>
        //public List<Transaction> Transactions { get; set; } = [];
        //public List<ScriptWritingPostByProducer> ScriptWritingPosts { get; set; } = [];
    }
}
