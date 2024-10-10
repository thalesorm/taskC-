﻿using ApiGap.Repository;
using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations;
using UserModel = ApiGap.Models.User;

namespace ApiGap.Services

{
    public class UserService
    {
        private readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<UserModel>> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsers();
            if (users == null || users.Count == 0)
            {
                throw new Exception("Nenhum usuário encontrado.");
            }
            return users;
        }

        public async Task<UserModel?> GetById(string id)
        {
            var user = await _userRepository.GetById(id);
            if (user == null)
            {
                throw new Exception("Usuário não encontrado.");
            }
            return user;
        }

        public async Task<UserModel> Create(UserModel user)
        {
            var validationContext = new ValidationContext(user);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(user, validationContext, validationResults, true);

            if (!isValid)
            {
                throw new Exception("Usuário inválido.");
            }

            return await _userRepository.Create(user);
        }

        public async Task<UserModel> Update(UserModel user, string id)
        {
            var existingUser = await _userRepository.GetById(id);
            if (existingUser == null)
            {
                throw new Exception($"Usuário com ID {id} não encontrado. Não é possível atualizar.");
            }

            return await _userRepository.Update(user, id);
        }

        public async Task<bool> Delete(string id)
        {
            var user = await _userRepository.GetById(id);
            if (user == null)
            {
                throw new Exception($"Usuário com ID {id} não encontrado. Não é possível deletar.");
            }

            return await _userRepository.Delete(id);
        }
    }

}
