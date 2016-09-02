﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericRegex
{
    class SeqExpr<T> : PatternExpr<T>
    {
        readonly List<PatternExpr<T>> expressions;

        public SeqExpr(IEnumerable<PatternExpr<T>> expressions)
        {
            this.expressions = expressions.ToList();
        }

        public IReadOnlyCollection<PatternExpr<T>> Expressions => expressions;

        internal override IEnumerable<MatchContext<T>> Match(MatchContext<T> context)
        {
            return Match(context, 0);
        }

        IEnumerable<MatchContext<T>> Match(MatchContext<T> context, int position)
        {
            if (position < expressions.Count)
            {
                foreach (var newContext in expressions[position].Match(context))
                {
                    foreach (var resultingContext in Match(newContext, position + 1))
                    {
                        yield return resultingContext;
                    }
                }
            }
            else
            {
                yield return context;
            }
        }

        public override string ToString() => $"Seq({string.Join(", ", expressions)})";
    }
}
