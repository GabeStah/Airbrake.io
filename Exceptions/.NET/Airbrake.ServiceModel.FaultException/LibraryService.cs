using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Utility;

namespace Airbrake.ServiceModel.FaultException
{
    // TODO: https://www.codeproject.com/Articles/799258/WCF-Exception-FaultException-FaultContract

    public interface IBook
    {
        string Author { get; set; }
        string Title { get; set; }
        bool Reserved { get; set; }
    }

    public class Book : IBook
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public bool Reserved { get; set; }

        public Book() { }

        public Book(string title, string author)
        {
            Title = title;
            Author = author;
        }
    }

    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface LibraryService
    {
        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        // TODO: Add your service operations here
        [OperationContract]
        [FaultContract(typeof(ArgumentExceptionFault))]
        bool ReserveBook(string title, string author);
    }

    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    // You can add XSD files into the project. After building the project, you can directly use the data types defined there, with the namespace "Airbrake.ServiceModel.FaultException.ContractType".
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }

    [DataContract]
    public class ArgumentExceptionFault
    {
        [DataMember]
        public bool Result { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public string Description { get; set; }
    }

    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class Library : LibraryService
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        public bool ReserveBook(string title, string author)
        {
            try
            {
                // Check if title value is null or has no characters.
                // Null must be checked prior to length to avoid checking an invalid object.
                if (title is null || title.Length == 0)
                {
                    throw new System.ArgumentException($"CheckoutBook() parameter 'title' must be a valid string.");
                }
                // Check if author value is null or has no characters.
                // Null must be checked prior to length to avoid checking an invalid object.
                if (author is null || author.Length == 0)
                {
                    throw new System.ArgumentException($"CheckoutBook() parameter 'author' must be a valid string.");
                }
                Book book = new Book(title, author);
                book.Reserved = true;
                Logging.Log(book);
                return true;
            }
            catch (ArgumentException e)
            {
                Logging.Log(e);

                var fault = new ArgumentExceptionFault();
                fault.Description = e.Message;
                fault.Message = e.Message;
                fault.Result = false;

                throw new FaultException<ArgumentExceptionFault>(fault);
            }
            catch (Exception e)
            {
                Logging.Log(e, false);
            }
            return false;
        }
    }
}
