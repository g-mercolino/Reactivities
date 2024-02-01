import { Grid } from "semantic-ui-react";
import ActivityList from "./ActivityList";
import { useStore } from "../../../app/stores/store";
import { observer } from "mobx-react-lite";
import { useEffect } from "react";
import LoadingComponent from "../../../app/layout/LoadingComponent";
import ActivityFilters from "./ActivityFilters";

//per non far crashare l'app verifichiamo prima se abbiamo un'attività con indice = 0
export default observer(function ActivityDashboard() {
    const {activityStore} = useStore();
    const {loadActivities, activityRegistry} = activityStore;

  //Axios è una libreria JavaScript che viene utilizzata per effettuare richieste HTTP
  useEffect(() => {
   if (activityRegistry.size <= 1) loadActivities();
  }, [loadActivities])

  if (activityStore.loadingInitial) return <LoadingComponent content='Loading app...'/>

    return (
        <Grid>
            <Grid.Column  width='10'>
                <ActivityList />         
            </Grid.Column>
            <Grid.Column  width='6'>
                <ActivityFilters/>
            </Grid.Column>
        </Grid>
    )
})