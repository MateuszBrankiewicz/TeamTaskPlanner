export interface CreateProject{
  name:string;
  description: string;
}

export interface UserInfoDto {
  id: number;
  name: string;
  email: string;
}

export interface ProjectMemberDto {
  id: number;
  userId: number;
  projectId: number;
  role: string;
  joinedDate: string;
  user: UserInfoDto;
}

export interface Projects {
title: any;
  id: number;
  name: string;
  description: string;
  createdDate: string;
  dueDate?: string;
  status: string;
  createdBy: UserInfoDto;
  members: ProjectMemberDto[];
  tasksCount: number;
  completedTasksCount: number;
}
