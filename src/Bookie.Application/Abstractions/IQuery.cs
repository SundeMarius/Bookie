using MediatR;

namespace Bookie.Application.Abstractions;

public interface IQuery<TResponse> : IRequest<TResponse>;