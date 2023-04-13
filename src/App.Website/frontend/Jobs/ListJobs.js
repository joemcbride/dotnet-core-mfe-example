import { useQuery, gql } from '@apollo/client'

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

  const jobs = data.jobs.all.map(({ jobId, status }) => (
    <li key={jobId}>
      {jobId} {status}
    </li>
  ))

  return (
    <>
      <h1>Jobs</h1>
      <ul>{jobs}</ul>
    </>
  )
}
