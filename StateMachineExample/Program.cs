using System;
using Automatonymous;

namespace StateMachineExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }

    class Relationship
    {
        public State CurrentState { get; set; }
        public string Name { get; set; }
    }

    public class FileState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; }

        public string FileName { get; set; }
    }
    public class OrderStateMachine : MassTransitStateMachine<FileState>
    {
        public State Received { get; private set; }
        public State Identified { get; private set; }
        public State Verified { get; private set; }
        public State Validated { get; private set; }
        public State Decrypted { get; private set; }
        public State Decompressed { get; private set; }
        public OrderStateMachine()
        {

            SubState(() => Decompressed, Verified);
            SubState(() => Decrypted, Verified);

            Initially(
                When(FileReceived)
                .Then (x => x.Instance.FileName = x.Data.FileName)
                .TransitionTo(Received));

            During(Received,
                When(FileIdentified)
                    .TransitionTo(Identified));

            During(Identified,
                When(FileVerified)
                    .TransitionTo(Verified));

            Event(() => FileReady, x => x.CorrelateById(context => context.Message.FileStorageId));

            //InstanceState(x => x.CurrentState);

            //Event(() => FileReady, x => x.CorrelateById(context => context.Message.FileStorageId));
        }

        public Event<FileReady> FileReceived { get; private set; }

        public Event<FileReady> FileReady { get; private set; }

        public Event<FileReady> FileIdentified { get; private set; }

        public Event<FileReady> FileVerified { get; private set; }

        public Event<FileReady> FileValidated { get; private set; }
    
    }

    public class FileReady
    {
        public Guid FileStorageId { get; }
        public string FileName { get; }
    }
}

