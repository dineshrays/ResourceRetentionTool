﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RetentionTool.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class RetentionToolEntities : DbContext
    {
        public RetentionToolEntities()
            : base("name=RetentionToolEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Commonfield> Commonfields { get; set; }
        public virtual DbSet<Skill> Skills { get; set; }
        public virtual DbSet<Module> Modules { get; set; }
        public virtual DbSet<AssignResource> AssignResources { get; set; }
        public virtual DbSet<AssignResourcesDet> AssignResourcesDets { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Manager> Managers { get; set; }
        public virtual DbSet<Trainer> Trainers { get; set; }
        public virtual DbSet<ModuleDet> ModuleDets { get; set; }
        public virtual DbSet<AssignEvaluater> AssignEvaluaters { get; set; }
        public virtual DbSet<EmployeeEvalTask> EmployeeEvalTasks { get; set; }
        public virtual DbSet<EmployeeEvalTaskDet> EmployeeEvalTaskDets { get; set; }
        public virtual DbSet<RateEmployeeEligiability> RateEmployeeEligiabilities { get; set; }
        public virtual DbSet<Session> Sessions { get; set; }
        public virtual DbSet<SessionsDet> SessionsDets { get; set; }
        public virtual DbSet<Training> Trainings { get; set; }
        public virtual DbSet<TrainingDet> TrainingDets { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<CurrentInfo> CurrentInfoes { get; set; }
        public virtual DbSet<EducationQualification> EducationQualifications { get; set; }
        public virtual DbSet<EmployeeSkill> EmployeeSkills { get; set; }
        public virtual DbSet<Experience> Experiences { get; set; }
        public virtual DbSet<ProjectsWorked> ProjectsWorkeds { get; set; }
        public virtual DbSet<UserDetail> UserDetails { get; set; }
        public virtual DbSet<PersonalInfo> PersonalInfoes { get; set; }
    }
}
