using System.Collections.Generic;
using MVC.Models; 

namespace MVC.Interfaces;

public interface IPresupuestoRepository
{
    List<Presupuesto> GetAll();

    Presupuesto GetById(int id);

    void Add(Presupuesto presupuesto);

    void Update(Presupuesto presupuesto);

    void Delete(int id);

    void AddDetalle(int idPresupuesto, int idProducto, int cantidad);
}
