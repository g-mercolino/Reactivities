import { useEffect, useState } from 'react';
import { Container } from 'semantic-ui-react';
import NavBar from './NavBar';
import ActivityDashboard from '../../features/activities/dashboard/ActivityDashboard';
import LoadingComponent from './LoadingComponent';
import { useStore } from '../stores/store';
import { observer } from 'mobx-react-lite';

function App() {
  const {activityStore} = useStore();

  //Axios è una libreria JavaScript che viene utilizzata per effettuare richieste HTTP
  useEffect(() => {
   activityStore.loadActivities();
  }, [activityStore])

  if (activityStore.loadingInitial) return <LoadingComponent content='Loading app'/>

  //   <h2>{activityStore.title}</h2>
  //<Button content='Add exclamation!' positive onClick={activityStore.setTitle}/>

  return (
    <>
        <NavBar />
        <Container style={{marginTop: '7em'}}>       
          <ActivityDashboard />
        </Container>
    </>
  )
}

export default observer(App);
