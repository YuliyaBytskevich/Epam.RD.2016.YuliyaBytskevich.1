﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Xml.Serialization;
using NLog;
using UserStorage.Predicates;
using UserStorage.Repositories;
using UserStorage.UserEntity;

namespace UserStorage.Services
{
    public abstract class UserStorageService: MarshalByRefObject, IUserStorageService
    {
        public ServiceState State { get; set; }
        protected ManualResetEvent collectionIsEnabled = new ManualResetEvent(true);
        private static readonly Logger logger = LogManager.GetLogger("*");    

        protected UserStorageService() { }

        protected UserStorageService(string serviceIdentifier, string xmlPath, IUserStorage storage)
        {
            State = new ServiceState(serviceIdentifier, xmlPath, 0, storage);
        }

        public void RestoreServiceState(IUserStorage targetStorage)
        {
            logger.Trace(State.Identifier + " : restoring State from xml-file... ");
            try
            {
                var serializer = new XmlSerializer(typeof (ServiceState));
                using (Stream fStream = new FileStream(State.XmlPath, FileMode.Open))
                {
                    State.SetTargerRepository(targetStorage);
                    State = (ServiceState) serializer.Deserialize(fStream);
                }
                logger.Trace("State has been restored successfully!\n");
            }
            catch (Exception ex)
            {
                logger.Error(ex.StackTrace + "\n");
            }
        }

        public void SaveServiceState()
        {
            logger.Trace(State.Identifier + " : saving State to xml-file... ");
            try
            {
                if (State != null)
                {
                    var serializer = new XmlSerializer(typeof (ServiceState));
                    using (Stream fStream = new FileStream(State.XmlPath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        serializer.Serialize(fStream, State);
                    }
                }
                logger.Trace("State has been saved successfully!\n");
            }
            catch (Exception ex)
            {
                logger.Error(ex.StackTrace + "\n");
            }
        }

        public abstract void Add(User user);

        public abstract void Delete(User user);

        public virtual int SearchForUser(params IPredicate[] predicates)
        {
            logger.Trace(State.Identifier + " : SEARCH operation called... ");
            var result = State.Repository.SearchForUser(predicates);
            if (result == -1)
                logger.Trace(State.Identifier + ": Repository is empty.\n");
            else if (result == 0)
                logger.Trace(State.Identifier + ": No matches found for given predicate(-s).\n");
            else
                logger.Trace(State.Identifier + ": Found user ID: " + result + "\n");
            return result;
        }

        public virtual List<int> SearchForUsers(params IPredicate[] predicates)
        {
            logger.Trace(State.Identifier + " : SEARCH operation called... ");
            var result = State.Repository.SearchForUsers(predicates);
            if (result == null)
                logger.Trace(State.Identifier + ": Repository is empty.\n");
            else if (!result.Any())
                logger.Trace(State.Identifier + ": No matches found for given predicate(-s).\n");
            else
            {
                logger.Trace(State.Identifier + ": Found user ID-s:");
                foreach (var element in result)
                {
                    logger.Trace(element);
                }
                logger.Trace("\n");
            }
            return result;
        }

    }

    public class StateObject
    {
        public Socket workSocket = null;
        public const int BufferSize = 1024;
        public byte[] buffer = new byte[BufferSize];
    }
}
