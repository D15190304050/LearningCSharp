using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Threading;

namespace NetworkConsole
{
    /// <summary>
    /// The ClientGetAsyncDemo class provides a demo for issuing the async request.
    /// </summary>
    public class ClientGetAsyncDemo
    {
        private class RequestState
        {
            private const int BufferSize = 1024;
            public StringBuilder RequestData { get; set; }
            public byte[] BufferRead { get; set; }
            public WebRequest Request { get; set; }
            public Stream ResponseStream { get; set; }

            // Create Decoder for appropriate encoding type.
            public Decoder StreamDecode { get; set; }

            public RequestState()
            {
                StreamDecode = Encoding.UTF8.GetDecoder();
                BufferRead = new byte[BufferSize];
                RequestData = new StringBuilder();
                Request = null;
                ResponseStream = null;
            }
        }

        public static ManualResetEvent allDone;
        private const int BufferSize = 1024;

        public static void Demo()
        {
            allDone = new ManualResetEvent(false);

            // Set the Uri.
            Uri httpSite = new Uri("http://www.contoso.com/");

            // Create the request object.
            WebRequest request = WebRequest.Create(httpSite);

            // Create a State object.
            RequestState state = new RequestState();

            // Pu the request into the State object so it can be passed arount.
            state.Request = request;

            // Issue the async request.
            IAsyncResult aresult = request.BeginGetResponse(ResponseCallback, state);

            // Wait until the ManualResetEvent is set so that the application does not exit until after the callback is called.
            allDone.WaitOne();

            // Write the received contents.
            Console.WriteLine(state.RequestData.ToString());
        }

        private static void ResponseCallback(IAsyncResult asyncResult)
        {
            // Get the RequestState object from the async result.
            RequestState state = (RequestState)asyncResult.AsyncState;

            // Get the WebRequest from RequestState.
            WebRequest request = state.Request;

            // Call the EndGetResponse(), which produces the WebResponse object that came from the request issued above.
            WebResponse response = request.EndGetResponse(asyncResult);

            // Start reading data from the response stream.
            Stream responseStream = response.GetResponseStream();

            // Store the response stream in RequestState object to read the stream asynchronously.
            state.ResponseStream = responseStream;

            // Pass state.BufferRead to BeginRead(). Read data into state.BufferRead.
            IAsyncResult iarRead = responseStream.BeginRead(state.BufferRead, 0, BufferSize, ReadCallback, state);
        }

        private static void ReadCallback(IAsyncResult asyncResult)
        {
            // Get the RequestState object from asyncResult.
            RequestState state = (RequestState)asyncResult.AsyncState;

            // Retrive the ResponseStream that was set in ResponseCallback().
            Stream responseStream = state.ResponseStream;

            // Read state.BufferRead to verify that it contains data.
            int read = responseStream.EndRead(asyncResult);
            if (read > 0)
            {
                // Prepare a char array buffer for converting to Unicode.
                char[] charBuffer = new char[BufferSize];

                // Append the recently read data to the RequestData StringBuilder object contained in RequestStare.
                state.RequestData.Append(Encoding.ASCII.GetString(state.BufferRead, 0, read));

                // Continue reading data until responseStream.EndRead() returns -1.
                IAsyncResult aresult = responseStream.BeginRead(state.BufferRead, 0, BufferSize, ReadCallback, state);
            }
            else
            {
                // Close down the response stream.
                responseStream.Close();

                // Set the ManualResetEvent so the main thread can exit.
                allDone.Set();
            }
        }
    }
}
