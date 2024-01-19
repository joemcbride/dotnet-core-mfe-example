import { useQuery, gql } from '@apollo/client'
import Header from '../components/Header'
import Table from '../components/Table'

const GET_JOBS = gql`
  query GetJobs {
    jobs {
      all {
        jobId
        status
      }
    }
  }
`

export default function ListJobs() {
  const { loading, error, data } = useQuery(GET_JOBS)

  console.log(loading, error, data)

  if (loading) return <p>Loading...</p>
  if (error) return <p>Error : {error.message}</p>

  const jobRows = data.jobs.all.map(({ jobId, status }) => (
    <Table.Row key={jobId}>
      <Table.Column>{jobId}</Table.Column>
      <Table.Column>{status}</Table.Column>
    </Table.Row>
  ))

  return (
    <div>
      <div className="sm:flex-auto">
        <Header>Jobs</Header>
        <p className="mt-2 text-sm text-gray-700">
          A list of all the jobs in the system.
        </p>
      </div>
      <div className="mt-8 flow-root">
        <div className="inline-block min-w-full">
          <div className="overflow-hidden shadow ring-1 ring-black ring-opacity-5 sm:rounded">
            <Table>
              <Table.Header>
                <Table.HeaderRow>Job Id</Table.HeaderRow>
                <Table.HeaderRow>Status</Table.HeaderRow>
              </Table.Header>
              <Table.Body>{jobRows}</Table.Body>
            </Table>
          </div>
        </div>
      </div>
    </div>
  )
}
