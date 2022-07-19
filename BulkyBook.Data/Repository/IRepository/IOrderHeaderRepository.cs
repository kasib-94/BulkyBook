﻿using BulkyBook.Models;

namespace BulkyBook.Data.IRepository;

public interface IOrderHeaderRepository : IRepository<OrderHeader>
{
    void Update(OrderHeader obj);
    void UpdateStatus(int id, string orderStatus,string? paymentStatus=null);
}