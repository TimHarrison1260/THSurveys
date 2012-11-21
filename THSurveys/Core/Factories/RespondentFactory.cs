using System;
using System.Collections.ObjectModel;

using Core.Model;

namespace Core.Factories
{
    public abstract class RespondentFactory
    {
        public abstract Respondent Create();
    }
}
