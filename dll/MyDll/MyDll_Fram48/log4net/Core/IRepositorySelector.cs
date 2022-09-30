// Decompiled with JetBrains decompiler
// Type: log4net.Core.IRepositorySelector
// Assembly: log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821
// MVID: 3FA2063C-770A-4FE1-BF9A-451CFF38D64F
// Assembly location: E:\LabCar\BasaDll\log4net.dll

using log4net.Repository;
using System;
using System.Reflection;

namespace log4net.Core
{
  public interface IRepositorySelector
  {
    ILoggerRepository GetRepository(Assembly assembly);

    ILoggerRepository GetRepository(string repositoryName);

    ILoggerRepository CreateRepository(Assembly assembly, Type repositoryType);

    ILoggerRepository CreateRepository(string repositoryName, Type repositoryType);

    bool ExistsRepository(string repositoryName);

    ILoggerRepository[] GetAllRepositories();

    event LoggerRepositoryCreationEventHandler LoggerRepositoryCreatedEvent;
  }
}
