﻿using ClaudeApi.Agents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaudeApi.Agents.User
{
    internal class ConsoleUserInterface : IUserInterface
    {
        private readonly StringBuilder _partialMessageBuilder = new ();

        public void AddArtifact(string artifact)
        {
            Console.WriteLine($"Artifact added: {artifact}");
        }

        public string Prompt(string message)
        {
            Console.WriteLine(message);
            ClearKeyboardBuffer();
            return Console.ReadLine() ?? "";
        }

        public void Message(string message)
        {
            Console.WriteLine(message);
        }

        public void ReceivePartialMessage(string partialMessage)
        {
            _partialMessageBuilder.Append(partialMessage);
            Console.Write(partialMessage);
        }

        public void EndPartialMessage()
        {
            Console.WriteLine(); // Move to the next line after the partial message is complete
            _partialMessageBuilder.Clear();
        }

        private static void ClearKeyboardBuffer()
        {
            while (Console.KeyAvailable)
            {
                Console.ReadKey(intercept: true);
            }
        }
    }
}