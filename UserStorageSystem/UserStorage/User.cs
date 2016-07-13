﻿using System;
using System.Linq;

namespace UserStorage
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PersonalId { get; set; }
        public Gender Gender { get; set; }
        public VisaRecord[] Visas { get; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            User user = obj as User;
            if ((System.Object)user == null)
            {
                return false;
            }
            return (PersonalId == user.PersonalId &&
                    DateOfBirth == user.DateOfBirth &&
                    Gender == user.Gender &&
                    LastName == user.LastName &&
                    FirstName == user.FirstName &&                   
                    AllVisasMatch(Visas, user.Visas)                    
                )? true: false;
        }

        public override int GetHashCode()
        {
            return (PersonalId.GetHashCode() ^ DateOfBirth.GetHashCode() ^ LastName.GetHashCode());
        }


        private bool AllVisasMatch(VisaRecord[] firstUserVisas, VisaRecord[] secondUserVisas)
        {
            if (firstUserVisas == null && secondUserVisas == null)
            {
                return true;
            }
            if (firstUserVisas == null || secondUserVisas == null)
            {
                return false;
            }
            if (firstUserVisas.Count() != secondUserVisas.Count())
            {
                return false;
            }
            int numOfVisas = firstUserVisas.Count();
            for (int i = 0; i < numOfVisas; i++)
            {
                if (!firstUserVisas[i].Equals(secondUserVisas[i]))
                {
                    return false;
                }
            }
            return true;
        }

    }
}