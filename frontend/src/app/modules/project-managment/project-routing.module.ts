import { RouterModule, Routes } from "@angular/router";
import { CreateProjectPageComponent } from "./create-project-page/create-project-page.component";
import { NgModule } from "@angular/core";

const routes : Routes = [
        {
          path: 'new-project',
          component: CreateProjectPageComponent
        }
      ]
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProjectRoutingModule {}
