﻿using AutoMapper;
using Maturidade_Online.Dominio.Usuario;
using Maturidade_Online.Models;
using Maturidade_Online.Servicos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Maturidade_Online.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            if (Session["USUARIO_LOGADO_CHAVE"] != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logar(UsuarioModel usuarioModel)
        {
            if (ModelState.IsValid)
            {
                UsuarioServico usuarioServico = ServicoDeDependencia.MontarUsuarioServico();

                var usuario = Mapper.Map<UsuarioModel, UsuarioEntidade>(usuarioModel);

                UsuarioEntidade usuarioEncontrado = usuarioServico.BuscarPorAutenticacao(usuario);

                if (usuarioEncontrado != null)
                {
                    ServicoDeAutenticacao.Autenticar(new UsuarioLogadoModel(
                        usuario.Email));
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Usuário ou Senha inválida.");

            }

            return View("Login");


        }

        public ActionResult Deslogar()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();

            return RedirectToAction("Login", "Login");
        }


    }
}