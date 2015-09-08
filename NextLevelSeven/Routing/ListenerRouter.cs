﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;

namespace NextLevelSeven.Routing
{
    /// <summary>
    /// Router that calls a method each time a message is passed through. 
    /// </summary>
    public class ListenerRouter : IRouter
    {
        /// <summary>
        /// Create a listener router that calls a method on all messages and then unconditionally passes them through.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="targetRouter"></param>
        public ListenerRouter(Action<IMessage> action, IRouter targetRouter = null)
        {
            Action = action;
            TargetRouter = targetRouter;
        }

        /// <summary>
        /// Method to relay messages to.
        /// </summary>
        public readonly Action<IMessage> Action;

        /// <summary>
        /// Router to route messages to when the condition is met.
        /// </summary>
        public IRouter TargetRouter;

        /// <summary>
        /// Route a message through the listener and to the target router.
        /// </summary>
        /// <param name="message">Message to route.</param>
        /// <returns></returns>
        public bool Route(IMessage message)
        {
            Action(message);
            return TargetRouter == null || TargetRouter.Route(message);
        }
    }
}