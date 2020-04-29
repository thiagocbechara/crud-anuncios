using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebmotorsTeste.Web.Data;
using WebmotorsTeste.Web.Infra;
using WebmotorsTeste.Web.Models;

namespace WebmotorsTeste.Web.Controllers
{
    public class AnunciosController : Controller
    {
        private readonly WebmotorsContext _context;
        private readonly IWebmotorsAPI _webmotorsAPI;

        public AnunciosController(WebmotorsContext context, IWebmotorsAPI webmotorsAPI)
        {
            _context = context;
            _webmotorsAPI = webmotorsAPI;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Anuncios.ToListAsync());
        }

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var anuncio = await _context.Anuncios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (anuncio == null)
            {
                return NotFound();
            }

            return View(anuncio);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Makes = await _webmotorsAPI.GetMakes();
            return View();
        }

        public async Task<IActionResult> GetModels(long makeId)
        {
            return Ok(await _webmotorsAPI.GetModels(makeId));
        }

        public async Task<IActionResult> GetVersions(long modelId)
        {
            return Ok(await _webmotorsAPI.GetVersions(modelId));
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Marca,Modelo,Versao,Ano,Quilometragem,Observacao")] Anuncio anuncio)
        {
            if (ModelState.IsValid)
            {
                await UpdateModelWithNames(anuncio);
                _context.Add(anuncio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(anuncio);
        }

        private async Task UpdateModelWithNames(Anuncio anuncio)
        {
            var makes = await _webmotorsAPI.GetMakes();
            var selectedMake = makes.FirstOrDefault(x => x.Id.ToString() == anuncio.Marca || x.Name == anuncio.Marca);
            
            var models = await _webmotorsAPI.GetModels(selectedMake.Id);
            var selectedModel = models.FirstOrDefault(x => x.Id.ToString() == anuncio.Modelo || x.Name == anuncio.Modelo);

            var verions = await _webmotorsAPI.GetVersions(selectedModel.Id);
            var selectedVersion = verions.FirstOrDefault(x => x.Id.ToString() == anuncio.Versao || x.Name == anuncio.Versao);

            anuncio.Marca = selectedMake.Name;
            anuncio.Modelo = selectedModel.Name;
            anuncio.Versao = selectedVersion.Name;
        }

        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var anuncio = await _context.Anuncios.FindAsync(id);
            if (anuncio == null)
            {
                return NotFound();
            }

            var makes = await _webmotorsAPI.GetMakes();
            var models = await _webmotorsAPI.GetModels(makes.FirstOrDefault(x => x.Name == anuncio.Marca).Id);
            var versions = await _webmotorsAPI.GetVersions(models.FirstOrDefault(x => x.Name == anuncio.Modelo).Id);

            ViewBag.Makes = makes.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name, Selected = x.Name == anuncio.Marca });
            ViewBag.Models = models.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name, Selected = x.Name == anuncio.Modelo });
            ViewBag.Versions = versions.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name, Selected = x.Name == anuncio.Versao });
            await UpdateModelWithIds(anuncio);
            return View(anuncio);
        }

        private async Task UpdateModelWithIds(Anuncio anuncio)
        {
            var makes = await _webmotorsAPI.GetMakes();
            var selectedMake = makes.FirstOrDefault(x => x.Id.ToString() == anuncio.Marca || x.Name == anuncio.Marca);

            var models = await _webmotorsAPI.GetModels(selectedMake.Id);
            var selectedModel = models.FirstOrDefault(x => x.Id.ToString() == anuncio.Modelo || x.Name == anuncio.Modelo);

            var verions = await _webmotorsAPI.GetVersions(selectedModel.Id);
            var selectedVersion = verions.FirstOrDefault(x => x.Id.ToString() == anuncio.Versao || x.Name == anuncio.Versao);

            anuncio.Marca = selectedMake.Id.ToString();
            anuncio.Modelo = selectedModel.Id.ToString();
            anuncio.Versao = selectedVersion.Id.ToString();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Marca,Modelo,Versao,Ano,Quilometragem,Observacao")] Anuncio anuncio)
        {
            if (id != anuncio.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await UpdateModelWithNames(anuncio);
                    _context.Update(anuncio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnuncioExists(anuncio.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(anuncio);
        }

        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var anuncio = await _context.Anuncios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (anuncio == null)
            {
                return NotFound();
            }

            return View(anuncio);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var anuncio = await _context.Anuncios.FindAsync(id);
            _context.Anuncios.Remove(anuncio);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnuncioExists(long id)
        {
            return _context.Anuncios.Any(e => e.Id == id);
        }
    }
}
