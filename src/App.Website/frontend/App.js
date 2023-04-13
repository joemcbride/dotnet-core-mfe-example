import CreateJob from './Jobs/CreateJob'
import ListJobs from './Jobs/ListJobs'

export default function App() {
  return (
    <div className="py-10">
      <main>
        <div className="mx-auto max-w-4xl sm:px-6 lg:px-8">
          <CreateJob />
          <ListJobs />
        </div>
      </main>
    </div>
  )
}
