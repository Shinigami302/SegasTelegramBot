using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SegasTelegramBotWebApplicationSP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SegasTelegramBotWebApplicationSP
{
    public class DataReader
    {
        private SegasBotContext _context;
        private DataReader _dataReader;

        private string users;
        private string reactions;
        private string howAreYou;
        private string answers;
        private string triggersAnswers;
        private string triggersHowAreYou;

        public DataReader GetDataReader
        {
            get 
            {
                if (null == _dataReader)
                {
                    _dataReader = new DataReader();
                    return _dataReader;
                }
                else
                {
                    return _dataReader;
                }
            }
        }
        public DataReader(SegasBotContext context)
        {
            _context = context;
            Init();
        }
        public DataReader()
        { 
        }

        public string[] GetUsers()
        {
            string[] val = users.Split(';');
            return val;
        }
        public string[] GetAnswers()
        {
            string[] val = answers.Split(';');
            return val;
        }
        public string[] GetReactions()
        {
            string[] val = reactions.Split(';');
            return val;
        }
        public string[] GetHowAreYou()
        {
            string[] val = howAreYou.Split(';');
            return val;
        }

        public string[] GetTriggersHowAreYou()
        {
            string[] val = triggersHowAreYou.Split(';');
            return val;
        }


        public string[] GetTriggersAnswers()
        {
            string[] val = triggersAnswers.Split(';');
            return val;
        }


        private void Init()
        {
            try
            {
                List<SBCommands> list = _context.SBCommands.ToList();
                if (1 == list.Count)
                {
                    users = list[0].Users;
                    reactions = list[0].Reactions;
                    howAreYou = list[0].HowAreYou;
                    answers = list[0].Answers;
                    triggersAnswers = list[0].TriggersAnswers;
                    triggersHowAreYou = list[0].TriggersHowAreYou;
                }
                else
                {
                    throw new Exception("Data was not initialized. Reason: List is empty or has more then one value!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Data was not initialized. Reason: ", ex);
            }
        }
    }
}
