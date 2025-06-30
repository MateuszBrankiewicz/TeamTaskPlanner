import { RouterModule, Routes } from "@angular/router";
import { CreateProjectPageComponent } from "./create-project-page/create-project-page.component";
import { NgModule } from "@angular/core";
import { ShowProjectsComponent } from "./show-projects/show-projects.component";

const routes : Routes = [
        {
          path: 'new-project',
          component: CreateProjectPageComponent
        },
        {
          path: '',
          component: ShowProjectsComponent
        }
      ]
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProjectRoutingModule {}
